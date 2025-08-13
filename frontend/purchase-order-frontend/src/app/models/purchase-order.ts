export type PurchaseOrderStatus =
  | 'Draft'
  | 'Approved'
  | 'Shipped'
  | 'Completed'
  | 'Cancelled';

export interface PurchaseOrder {
  id: string;
  poNumber: string;
  description?: string;
  supplierName: string;
  orderDate: string; // ISO date string
  totalAmount: number;
  status: PurchaseOrderStatus;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
