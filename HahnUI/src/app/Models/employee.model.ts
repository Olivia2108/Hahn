export interface Employee {
    id: string,
    name: string,
    email: string,
    phone: string,
    salary: string,
    department: string, 
    dateCreated: Date, 
    ipAddress: string
  }
  
export interface newOrder {
    shippingAdress: string,
    orderItemsDtoModel: 
    [
      {
        productName: string,
        price: {
          amount: number,
          unit: number
          }
      }
     
    ]
}
