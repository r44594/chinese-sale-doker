
//interface-לשים לב הרבה יותר נכון ובטיחותי להתשתמש בממשק מפני שהנתוננים נמחקים מייד אחרי השימוש ואין רגישות לאותיות גדולות וקטנות וכו..
export interface RegisterDto {
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
}

export interface LoginDto {
  userName: string;
  password: string;
}

export interface GetUsers {
  id: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: string; 
}

export interface LoginResponseDto {
  token: string;
  tokenType: string;
  expiresIn: number; 
  user: GetUsers;
}


