import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../services/order.service';

import { RandomService } from '../../services/random.service';
import { RandomDto } from '../../models/random.model';
import { DialogModule } from 'primeng/dialog';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog'
import { BuyerOrderDto } from '../../models/order.model';
import { MostPurchasedGiftDto } from '../../models/gift.model';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ConfirmationService, MessageService } from 'primeng/api';
@Component({
  selector: 'app-order',
  imports: [CommonModule, FormsModule ,DialogModule,
    TableModule,
    ButtonModule,TagModule,
    TooltipModule,ProgressSpinnerModule,ConfirmDialogModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})

export class OrderComponent {
 orderService = inject(OrderService);
 randomService=inject(RandomService);
 messageService = inject(MessageService);
 confirmationService = inject(ConfirmationService);
  buyers: BuyerOrderDto[] = [];
   filteredGifts: MostPurchasedGiftDto[] = [];
  specificBuyers: BuyerOrderDto[] = [];
winners: RandomDto[] = [];
  totalIncome: number = 0;
  isLoading = false;
  currentView: 'winners'|'popular' | 'buyers' = 'popular';
  displayBuyersDialog = false;

  ngOnInit(): void {
    this.loadMostPurchased();
  }

loadMostPurchased(): void {
  this.isLoading = true;
  this.currentView = 'popular';
  this.orderService.getMostPurchasedGift().subscribe({
    next: (data) => {
    
      this.filteredGifts = Array.isArray(data) ? data : [data]; 
      this.isLoading = false;
    },
   error: (err) => {
        this.isLoading = false;
        if (err.status === 401) {
          console.warn('הגישה לנתונים אלו דורשת התחברות, אך הדף מוגדר כציבורי.');
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'שגיאה',
            detail: 'טעינת המתנות נכשלה',
            styleClass: 'black-toast'
          });
        }
      }
    });
  }

  loadWinnersReport(): void {
    this.isLoading = true;
    this.currentView = 'winners';
    this.randomService.GetWinnersReport().subscribe({
     next: (data) => {
        this.winners = data;
        this.randomService.GetTotalIncome().subscribe({
          next: (incomeData) => {
           this.totalIncome = incomeData.totalIncome ?? (incomeData as any).TotalIncome ?? 0;
            this.isLoading = false;
          },
          error: () => {
            this.isLoading = false;
            this.messageService.add({ severity: 'warn', summary: 'שים לב', detail: 'הזוכים נטענו אך סך ההכנסות לא זמין', styleClass: 'black-toast' });
          }
        });
      },
      error: () => {
        this.isLoading = false;
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'נכשלה שליפת דוח הזוכים', styleClass: 'black-toast' });
      }
    });
  }

  loadAllBuyers(): void {
    this.isLoading = true;
    this.currentView = 'buyers';
    this.orderService.getBuyerDetails().subscribe({
      next: (data) => {
        this.buyers = data;
        this.isLoading = false;
      },
     error: () => {
        this.isLoading = false;
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'לא ניתן לטעון את רשימת הרוכשים', styleClass: 'black-toast' });
      }
    });
  }

  openSpecificGiftBuyers(id: number): void {
    this.orderService.getBuyerDetails().subscribe({
      next: (allBuyers) => {
        console.log('כל הרכישות שהגיעו מהשרת:', allBuyers); 
        if (this.specificBuyers.length === 0) {
          this.messageService.add({ severity: 'info', summary: 'מידע', detail: 'אין רוכשים רשומים למתנה זו', styleClass: 'black-toast' });
        }
        this.specificBuyers = allBuyers.filter(buyer => buyer.giftId === id);
        this.displayBuyersDialog = true;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'תקלה בשליפת רוכשי המתנה', styleClass: 'black-toast' });
      }
    });
  }

  sortGifts(criteria: 'price' | 'purchased') {
    if (criteria === 'price') {
      this.filteredGifts.sort((a, b) => b.ticketPrice - a.ticketPrice);
    } else {
      this.filteredGifts.sort((a, b) => b.totalQuantity - a.totalQuantity);
    }
  }




random() {
    this.confirmationService.confirm({
      message: 'האם אתה בטוח שברצונך להריץ הגרלה? פעולה זו סופית!',
      header: 'אישור הגרלה',
      icon: 'pi pi-exclamation-circle',
      acceptLabel: 'כן, בצע הגרלה',
      rejectLabel: 'ביטול',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.isLoading = true;
        this.randomService.DrawLottery().subscribe({
          next: (data) => {
            console.log("זוכים שהתקבלו מהשרת:", data);
            this.winners = data;
            this.currentView = 'winners';
         
            
            this.loadWinnersReport(); 
          this.loadMostPurchased();
            this.messageService.add({ severity: 'success', summary: 'ההגרלה בוצעה', detail: `הוגרלו בהצלחה ${data.length} מתנות`, styleClass: 'black-toast' });
            this.isLoading = false;
          },
          error: (err) => {
            this.isLoading = false;
            this.messageService.add({ severity: 'error', summary: 'שגיאת הגרלה', detail: err.error?.message || 'הפעולה נכשלה', styleClass: 'black-toast' });
          }
        });
      }
    });
  }
}




