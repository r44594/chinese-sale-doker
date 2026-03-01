import { CommonModule } from '@angular/common'; 
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { Component } from '@angular/core';
import { BasketService } from '../../services/basket.service';
import { inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { BasketDto, CheckoutDto } from '../../models/basket.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-basket',
  imports: [CommonModule, 
    TableModule, 
    ButtonModule, 
    CardModule,RouterLink,ToastModule,FormsModule],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})

export class BasketComponent {
  basketService = inject(BasketService);
  basketItems: any[] = [];
  totalAmount = 0;
  creditCard: string = '';
  userId: number = 0; 
router = inject(Router);
messageService = inject(MessageService);
ngOnInit() {
    const savedId = localStorage.getItem('userId');
  if (savedId) {
      this.userId = Number(savedId);
      this.moveItemsToServer();
    } else {
      this.messageService.add({ severity: 'info', summary: 'הזדהות נדרשת', detail: 'יש להתחבר כדי להמשיך ברכישה' });
      this.router.navigate(['/login']);
    }
  }
  moveItemsToServer() {
    const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
    
    if (localCart.length > 0) {
      localCart.forEach((item: any, index: number) => {
        const dto: BasketDto = {
          userId: this.userId,
          giftId: item.giftId,
          quantity: item.quantity
        };
        this.basketService.addToBasket(dto).subscribe({
          next: () => {
           
            if (index === localCart.length - 1) {
              localStorage.removeItem('cart');
              this.loadBasket();
            }
          },
          error: () => this.loadBasket() 
        });
      });
    } else {
      this.loadBasket();
    }
  }
loadBasket() {
  if (this.userId === 0)
     return;

  this.basketService.GetBasketByUserIdAsync(this.userId).subscribe({
    next: (data) => {
      console.log('מה הגיע מהשרת?', data); 
      this.basketItems = data?.basketItem || []; 
      
      this.zoverallmony();
    },
    error: (err) => {
      this.messageService.add({ 
          severity: 'error', 
          summary: 'שגיאה', 
          detail: 'לא הצלחנו לטעון את הסל מהשרת',
          styleClass: 'black-toast' 
        });
      this.basketItems = [];
    }
  }); 
}
zoverallmony() {
  this.totalAmount = this.basketItems.reduce(
    (sum, item) => sum + ((item.gift?.ticketPrice || 0) * (item.quantity || 0)),
    0
  );
}

processCheckout() {
  if (!this.creditCard || this.creditCard.length !== 16) {
      this.messageService.add({ 
        severity: 'warn', 
        summary: 'נתוני תשלום', 
        detail: 'יש להזין כרטיס אשראי בעל 16 ספרות',
        styleClass: 'black-toast'
      });
      return;
    }

    const checkoutData: CheckoutDto = {
      userId: this.userId,
      creditCard: this.creditCard
    };
    this.basketService.checkout(checkoutData).subscribe({
      next: (response) => {
       this.messageService.add({ 
          severity: 'success', 
          summary: 'הרכישה הושלמה!', 
          detail: 'מספר הזמנה: ' + response.orderId                      
        });
        this.basketItems = [];
        this.totalAmount = 0;
        this.creditCard = '';
        setTimeout(() => this.router.navigate(['/gifts']), 2000)
        },
      error: (err) => {
        const errorMsg = err.error?.message || 'אירעה שגיאה בעיבוד התשלום';
        this.messageService.add({ 
          severity: 'error', 
          summary: 'שגיאה ברכישה', 
          detail: errorMsg,
          styleClass: 'black-toast' 
        });
      }
    });
  }
 
  removeItem(basketItemId: number) {
    this.basketService.removeBasketItem(basketItemId).subscribe({
      next: () => {
        this.messageService.add({ severity: 'info', summary: 'הוסר', detail: 'הפריט הוסר מהסל' });
        this.loadBasket();
      },
      error: (err) => {
       const msg = typeof err.error === 'string' ? err.error : 'לא ניתן למחוק לאחר רכישה';
        this.messageService.add({ 
          severity: 'error', 
          summary: 'חסימת מחיקה', 
          detail: msg,
          styleClass: 'black-toast' 
        });
      }
    });
  }
}
