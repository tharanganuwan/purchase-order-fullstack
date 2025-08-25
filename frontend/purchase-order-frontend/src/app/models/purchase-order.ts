export enum PurchaseOrderStatus {
  Draft = 0,
  Approved = 1,
  Shipped = 2,
  Completed = 3,
  Cancelled = 4
}

export interface PurchaseOrder {
  id: string;
  poNumber: string;
  description?: string;
  supplierName: string;
  orderDate: string; 
  totalAmount: number;
  status: PurchaseOrderStatus;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
