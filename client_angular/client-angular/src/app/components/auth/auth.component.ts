import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms'; 
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { PasswordModule } from 'primeng/password';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink,InputTextModule,
    ButtonModule,
    PasswordModule,
    CardModule,
    ToastModule,
    IconFieldModule,
    InputIconModule], 
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private messageService = inject(MessageService);
  isLoginMode: boolean = true;
  isLoading: boolean = false; 

  userAuth = {
    userName: '',
    password: '',
    email: '',
    firstName: '',
    lastName: '',
    phone: ''
  };

  ngOnInit(): void {
    
    this.route.url.subscribe(url => {
      this.isLoginMode = url[0].path === 'login';
      this.resetForm();
    });
  }

  onSubmit(form: NgForm) {

   if (form.invalid) {
      this.messageService.add({ 
        severity: 'warn', 
        summary: 'טופס לא תקין', 
        detail: 'נא למלא את כל השדות כנדרש', 
        styleClass: 'black-toast' 
      });
      return;
    }

    this.isLoading = true;
    if (this.isLoginMode) {
      
      this.authService.login({ 
        userName: this.userAuth.userName, 
        password: this.userAuth.password 
      }).subscribe({
        next: (res) => {  this.isLoading = false;
          if (!res || res === null) {
          this.messageService.add({ 
            severity: 'error', 
            summary: 'כניסה נכשלה', 
            detail: 'שם משתמש או סיסמה אינם קיימים במערכת',
            styleClass: 'black-toast'
          });
          return; 
        }
          this.router.navigate(['/gifts'])},
        error: (err) => {
          this.isLoading = false;
        this.messageService.add({ 
          severity: 'error', 
          summary: 'כניסה נכשלה', 
          detail: 'שם משתמש או סיסמה לא קיימים במערכת' ,
          styleClass: 'black-toast'
        });
        }
      });
    } else {
      this.authService.register({
        userName: this.userAuth.userName,
        password: this.userAuth.password,
        email: this.userAuth.email,
        firstName: this.userAuth.firstName,
        lastName: this.userAuth.lastName,
        phone: this.userAuth.phone
      }).subscribe({
        next: () => {
          this.isLoading = false;
          this.messageService.add({ severity: 'success', summary: 'מזל טוב!', detail: 'נרשמת בהצלחה, כעת התחבר' });
          this.router.navigate(['/login'])
        },
        error: () => {
          this.isLoading = false;
          this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'רישום נכשל. וודא שכל השדות תקינים' });
        }
      });
    }
  }

resetForm() {
    this.userAuth = {
      userName: '',
      password: '',
      email: '',
      firstName: '',
      lastName: '',
      phone: ''
    };
  }

  
  }
