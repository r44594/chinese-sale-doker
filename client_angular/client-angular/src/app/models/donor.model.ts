
export interface GiftDonor {
    id: number;          
    giftName: string;    
    ticketPrice: number;  
    countOfSale: number;
}

export interface GetDonor {
    id: number;           
    firstName: string;   
    lastName: string;     
    email: string;   
    phone: string;     
    gifts: GiftDonor[];  
}

export interface CreateUpdateDonor {
    id?: number;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
}