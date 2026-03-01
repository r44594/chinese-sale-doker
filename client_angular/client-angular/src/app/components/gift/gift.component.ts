import { Component, inject } from '@angular/core'; 
import { AsyncPipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GiftService } from '../../services/gift.service';
import { CreateUpdate, Get } from '../../models/gift.model';
import { Router } from '@angular/router';
import { BasketDto } from '../../models/basket.model';
import { BasketService } from '../../services/basket.service';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { Textarea } from 'primeng/textarea';
import { DonorService } from '../../services/donor.service';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast'; 
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
export enum Role {
  User,
  Admin
}
@Component({
  selector: 'app-gift',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    CardModule,
    RippleModule,
    TagModule,      
Textarea ,
InputNumberModule,
AsyncPipe,
DropdownModule,
ToastModule,
  ],
  templateUrl: './gift.component.html',
  styleUrls: ['./gift.component.css']
})


export class GiftComponent {
  basketService = inject(BasketService);
    giftService: GiftService = inject(GiftService);
donorService = inject(DonorService);
router = inject(Router);
 messageService = inject(MessageService);


 gifts$!: Observable<Get[]>;
  donors$: Observable<any> | null = null;

categories$!: Observable<any>;
userRole: Role = localStorage.getItem('role') === 'Admin' ? Role.Admin : Role.User;
  readonly Role = Role;

  showAddGift = false;
  isEditMode = false;

 
newGift: any = {
    id: 0,
    giftName: '',
    description: '',
    ticketPrice: 0,
    categoryId: 0, 
    donorId: 0,     
    imageUrl: '',
    count: 0
};

 
  filter = {
    name: '',
    donorName: '',
    minPurchasers: null,
    categoryName: ''
  };
ngOnInit() {
   this.refreshGifts();
   if (this.userRole === Role.Admin) {
    this.donors$ = this.donorService.getAllDonors();
  }
  this.categories$ = this.giftService.getAllCategories();
  }
 
  openAddGift() {
    this.isEditMode = false;
    this.resetForm();
    this.donors$ = this.donorService.getAllDonors();
    this.categories$ = this.giftService.getAllCategories();
    this.showAddGift = true;
  }

editGift(g: any) {
    this.isEditMode = true;
    this.donors$ = this.donorService.getAllDonors();
    this.categories$ = this.giftService.getAllCategories();

    this.newGift = { ...g }; 
   
    this.newGift.id = g.id || g.Id;
    this.newGift.giftName = g.giftName || g.GiftName;
    this.newGift.ticketPrice = g.ticketPrice || g.TicketPrice;
    this.newGift.categoryId = g.categoryId || g.CategoryId;
    this.newGift.donorId = g.donorId || g.DonorId;
    this.newGift.imageUrl = g.imageUrl || g.ImageUrl;
    this.newGift.count = 0;

    this.showAddGift = true;
}
  refreshGifts() {
    this.gifts$ = this.giftService.getAllGifts();
    this.resetForm();
  }
saveGift() {
  if (!this.newGift.giftName || !this.newGift.donorId || !this.newGift.categoryId|| !this.newGift.imageUrl) {
   this.messageService.add({ severity: 'warn', summary: 'שדות חסרים', detail: 'חובה למלא את כל פרטי המתנה' });
    return;
  }

  if (this.newGift.ticketPrice < 1 || this.newGift.ticketPrice > 1000) {
   this.messageService.add({ 
      severity: 'error', 
      summary: 'מחיר לא תקין', 
      detail: 'המחיר חייב להיות בין 1 ל-1000 ש"ח', 
      styleClass: 'black-toast' 
    });
    return;
  }

    const giftToSave: CreateUpdate = {
      giftName: this.newGift.giftName,
      description: this.newGift.description|| '',
      ticketPrice: this.newGift.ticketPrice,
      categoryId: Number(this.newGift.categoryId), 
  donorId: Number(this.newGift.donorId),
      imageUrl: this.newGift.imageUrl
    };

    
  if (this.isEditMode) {
   
    this.giftService.updateGift(this.newGift.id, giftToSave as any).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'עודכן', detail: 'המתנה עודכנה בהצלחה', styleClass: 'black-toast' });
        this.refreshGifts();
        this.showAddGift = false;
      },
    error: (err) => {
        console.error('Update Error:', err);
        const detail = err.status === 403 ? 'אין לך הרשאת מנהל לעדכן!' : 'שגיאה בעדכון המוצר';
        this.messageService.add({ severity: 'error', summary: 'אופס...', detail, styleClass: 'black-toast' });
      }
    });
    } 
    else {
        
        this.giftService.addGift(giftToSave).subscribe({
            next: () => {
                this.messageService.add({ 
                    severity: 'success', 
                    summary: 'נשמר', 
                    detail: 'מתנה חדשה נוספה למכירה', 
                    styleClass: 'black-toast' 
                });
                this.refreshGifts();
                this.showAddGift = false;
            },
            error: (err) => {
                console.error('Add Error:', err);
                let detail = 'שגיאה בהוספת המתנה';
                
                if (err.status === 400 && err.error?.errors) {
                    detail = Object.values(err.error.errors)[0] as string;
                } else if (err.status === 403) {
                    detail = 'פעולה חסומה: נדרשת גישת מנהל';
                }
                
                this.messageService.add({ severity: 'error', summary: 'שגיאה', detail, styleClass: 'black-toast' });
            }
        });
    }
  }
  
resetForm() {
    this.newGift = {
        id: 0,
        giftName: '',
        description: '',
        ticketPrice: 0,
        categoryId: 0,
        donorId: 0,
        imageUrl: '',
        count: 0
    };
}
  increment(g: Get) {
    g.count = (g.count || 0) + 1;
  }

  decrement(g: Get) {
    if (g.count && g.count > 0) {
      g.count--;
    }
  }

  deleteGift(id: number) {
    this.giftService.deleteGift(id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'info', summary: 'נמחק', detail: 'המתנה הוסרה בהצלחה' });
        this.refreshGifts();
      },
   error: (err) => {
      const serverMsg = err.error?.message || err.error;
      let msg = (typeof serverMsg === 'string') ? serverMsg : 'חלה שגיאה במחיקה';
     if (err.status === 409) {
        msg = 'לא ניתן למחוק מתנה שיש לה כבר רוכשים רשומים.';
      } else if (err.status === 403) {
        msg = 'אין לך הרשאה למחוק מתנות';
      } else if (err.status === 401) {
        msg = 'יש להתחבר מחדש למערכת';
      }
      this.messageService.add({ 
        severity: 'error', 
        summary: 'שגיאה במחיקה', 
        detail: msg, 
        styleClass: 'black-toast', 
        life: 5000 
      });
    }
  });
}

  onFilterChange() {

    if (this.filter.name && this.filter.name.trim() !== '') {
      this.gifts$ = this.giftService.searchGiftsByName(this.filter.name);
    }
 
    else if (this.filter.donorName && this.filter.donorName.trim() !== '') {
      this.gifts$ = this.giftService.searchByDonor(this.filter.donorName);
    }
   
    else if (this.filter.categoryName && this.filter.categoryName.trim() !== '') {
      this.gifts$ = this.giftService.getGiftsByCategory(this.filter.categoryName);
    }
  
    else if (this.filter.minPurchasers !== null && this.filter.minPurchasers !== undefined) {
      this.gifts$ = this.giftService.GetByBuyersCount(this.filter.minPurchasers);
    }
  
    else {
      this.gifts$ = this.giftService.getAllGifts();
    }
  }

 
  sortByPrice() {
    this.gifts$ = this.giftService.GetOrderedByPrice();
  }

  clearFilter() {
    
    this.filter = { name: '', donorName: '', minPurchasers: null, categoryName: '' };
    this.gifts$ = this.giftService.getAllGifts();
  }
 trackById(index: number, item: Get): number {
  return item.id;
}
goToCart() {
this.router.navigate(['/basket']);
}

 addToCart(g: Get) {
  if (this.userRole === Role.Admin) {
      this.messageService.add({ 
        severity: 'error', 
        summary: 'גישה חסומה', 
        detail: 'מנהל מערכת אינו רשאי לבצע רכישות' 
      });
      return;
    }
   if (!g.count || g.count <= 0) {
      this.messageService.add({ severity: 'warn', summary: 'כמות חסרה', detail: 'בחר לפחות כרטיס אחד' });
      return;
    }
  
  
  if (g.isDrawn ) {
    this.messageService.add({ 
      severity: 'error', 
      summary: 'המכירה נסגרה', 
      detail: `לא ניתן להוסיף את ${g.giftName} - המתנה כבר הוגרלה!`,
      styleClass: 'black-toast',
      life: 4000
    });
    return; 
  }
    let cart = JSON.parse(localStorage.getItem('cart') || '[]');
   const existingItem = cart.find((item: any) => item.giftId === g.id);
  if (existingItem) {
    existingItem.quantity += g.count;
  }
    else {
   cart.push({
        giftId: g.id,
        giftName: g.giftName,
        quantity: g.count,
        price: g.ticketPrice,
        imageUrl: g.imageUrl
      });
  }
  localStorage.setItem('cart', JSON.stringify(cart));
  this.messageService.add({ 
      severity: 'success', 
      summary: 'נוסף לסל!', 
      detail: `${g.giftName} (כמות: ${g.count}) נוספה לסל הקניות שלך`,
      icon: 'pi pi-shopping-cart',
      life: 3000
    });
    g.count = 0;
}
      
  
  

getUserName(): string {
  return localStorage.getItem('userName') || '';
}

getUserRole(): string {
  return localStorage.getItem('role') || '';
}



isLoggedIn(): boolean {
  return !!localStorage.getItem('token');
}
handleImageError(event: any) {
  
 event.target.src = 'assets/images/default.jpg';
}
navToStats(giftId: number) {
   
    this.router.navigate(['/order', giftId]);
  }
}


