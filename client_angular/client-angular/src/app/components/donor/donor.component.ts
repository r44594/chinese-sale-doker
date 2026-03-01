import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DonorService } from '../../services/donor.service';
import { CreateUpdateDonor, GetDonor  } from '../../models/donor.model';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-donor',
  imports: [CommonModule, FormsModule, 
    DialogModule, ButtonModule, InputTextModule, CardModule, TagModule,ToastModule],
  templateUrl: './donor.component.html',
  styleUrl: './donor.component.css'
})
export class DonorComponent {
 donorService = inject(DonorService);
 messageService = inject(MessageService);
 showDonorDialog = false;
 isEditMode = false;
donors$!: Observable<GetDonor[]>;
  newDonor: CreateUpdateDonor = { id: 0, firstName: '', lastName: '', email: '', phone: '' };

  filter = { name: '', email: '', giftName: '' };
  
  ngOnInit() {
    this.refresh(); 
  }

  onFilterChange() {
    if (this.filter.name) {
      this.donors$ = this.donorService.getDonorByName(this.filter.name);
    } else if (this.filter.email) {
      this.donors$ = this.donorService.getDonorsByEmail(this.filter.email);
    } else if (this.filter.giftName) {
      this.donors$ = this.donorService.getDonorsByGiftName(this.filter.giftName);
    } else {
      this.donors$ = this.donorService.getAllDonors();
    }
  }

  openAddDonor() {
    this.isEditMode = false;
    this.newDonor = { id: 0, firstName: '', lastName: '', email: '', phone: '' };
    this.showDonorDialog = true;
  }

editDonor(d: GetDonor) {
    this.isEditMode = true;
    
    this.newDonor = { 
      id: d.id, 
      firstName: d.firstName,
      lastName: d.lastName,
      email: d.email,
       phone: d.phone
    };
    this.showDonorDialog = true;
  }
 

  saveDonor() {
    if (!this.newDonor.firstName || !this.newDonor.lastName || !this.newDonor.email) {
      this.messageService.add({ severity: 'warn', summary: 'חסרים פרטים', detail: 'נא למלא את כל שדות החובה', styleClass: 'black-toast' });
      return;
    }
  if (this.isEditMode && this.newDonor.id !== undefined) {
    this.donorService.updateDonor(this.newDonor.id, this.newDonor).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'עודכן', detail: 'פרטי התורם עודכנו', styleClass: 'black-toast' });
        this.refresh();
      }
    });
  } else {
    this.donorService.addDonor(this.newDonor).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'נשמר', detail: 'תורם חדש נוסף למערכת', styleClass: 'black-toast' });
        this.refresh();
      }
    });
  }
  this.showDonorDialog = false;
}
deleteDonor(donor: GetDonor) {
    const hasPurchasedTickets = donor.gifts.some(g => g.countOfSale > 0);
    if (hasPurchasedTickets) {
      this.messageService.add({ 
        severity: 'error', 
        summary: 'לא ניתן למחוק', 
        detail: 'לתורם זה יש מתנות שכבר נרכשו עבורן כרטיסים!',
        styleClass: 'black-toast' 
      });
      return;
    }
    this.donorService.deleteDonor(donor.id).subscribe({
      next: () => {
        this.messageService.add({ 
          severity: 'success', 
          summary: 'נמחק', 
          detail: 'התורם הוסר בהצלחה', 
          styleClass: 'black-toast' 
        });
        this.refresh();
      },
      error: (err) => {
        this.messageService.add({ 
          severity: 'error', 
          summary: 'שגיאת מערכת', 
          detail: err.error || 'נכשלה מחיקת התורם', 
          styleClass: 'black-toast' 
        });
      }
    });
  }


  refresh() {
    this.donors$ = this.donorService.getAllDonors();
  }

  clearFilter() {
    this.filter = { name: '', email: '', giftName: '' };
    this.refresh();
  }
 
}

