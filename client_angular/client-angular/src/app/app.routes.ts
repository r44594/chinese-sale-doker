import { Routes } from '@angular/router';
import { GiftComponent } from './components/gift/gift.component';
import { AuthComponent } from './components/auth/auth.component';
import { DonorComponent } from './components/donor/donor.component';
import { BasketComponent } from './components/basket/basket.component';
import { OrderComponent } from './components/order/order.component';

export const routes: Routes = [ 
  { path: '', redirectTo: 'gifts', pathMatch: 'full' } ,
  { path: 'gifts', component: GiftComponent },
  { path: 'donors', component: DonorComponent },
  { path: 'login', component: AuthComponent },
  { path: 'register', component: AuthComponent },
  { path: 'basket', component: BasketComponent },
  {path:'gift-details/:giftId',component:OrderComponent},
  {path:'order',component:OrderComponent},
  { path: 'order/:id', component: OrderComponent },
 //כמו שהמורה הראתה בשיעור שתופס את האחרון שאין לו ניתוב מוסמך אנגולאר יביא לי את המשתמש
 //  לפוא תמיד לא משנה מה ירשום ביוארל גם שטיות תמיד יביא אותי  למתנות
 { path: '**', redirectTo: 'gifts', pathMatch: 'full' }
];

