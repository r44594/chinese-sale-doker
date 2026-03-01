

export interface OrderItemDto {
    id: number;
    giftName: string;
    ticketPrice: number;
}

export interface OrderDto {
    id: number;
    items: OrderItemDto[];
}

export interface BuyerOrderDto {
    orderId: number;
    giftId:number;
    orderDate: Date;
    userId: number;
    firstName: string;
    lastName: string;
     giftName: string;
    userName?: string; 
    email: string;
    phone: string;
}

