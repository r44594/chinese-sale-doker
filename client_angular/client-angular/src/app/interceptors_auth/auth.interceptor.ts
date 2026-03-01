//לשים לב ככה כל הקומוננטות מכירות את פרטי הטוקןן
import { inject, Injectable } from '@angular/core';
import {
  HttpErrorResponse,
    HttpEvent,
    HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { MessageService } from 'primeng/api';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
// ngOnInit()---  intercept--זהו פונקציה כמו האיתחול בתחילת הקומפוננטה שממשק זה מחייב אותי
//   intercept-היא תופסת את כל בקשות ה-HTTP-ולפני שה שהן יוצאות לשרת הוא נותן דרך לשנות אותןן
private messageService = inject(MessageService);
 intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');
let request = req;
     // אם יש טוקן – מוסיף Authorization
  if (token) {
      request = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          if (token) {
            this.messageService.add({ 
              severity: 'error', 
              summary: 'שגיאת הזדהות', 
              detail: 'פג תוקף החיבור, נא להיכנס מחדש', 
              styleClass: 'black-toast' 
            });
          }
        }
        return throwError(() => error);
      })
    );
  }
}

