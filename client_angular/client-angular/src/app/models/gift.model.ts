
export interface Get {
  id: number;
  giftName: string;
  description?: string;
  ticketPrice: number;
  imageUrl: string;
  categoryId: number;
  categoryName?: string;
  donorId: number;      
  isDrawn: boolean;
  winnerName?: string;
  count: number;
  donorName?: string;
}



export interface CreateUpdate {
  giftName: string;     
  description?: string; 
  ticketPrice: number;  
  imageUrl: string;   
  categoryId: number;   
  donorId: number;    
}

  export interface MostPurchasedGiftDto {
    giftId?: number;
    giftName: string;
    ticketPrice: number;
    totalQuantity: number;
    IsDrawn : boolean;
  }
           

  export interface AfterRandom {
    id: number;
    giftName: string;
    description?: string;
    ticketPrice: number;
    imageUrl?: string;
    categoryId: number;
    donorId: number;
    isDrawn: boolean;
    winnerName?: string;
    winnerUserId?: number;
    categoryName?: string;
  }
export interface CategoryDto {
  id: number;
  name: string;
}

