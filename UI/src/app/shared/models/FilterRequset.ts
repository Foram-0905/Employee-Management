export interface RequestWithFilterAndSort {
  filterModel?:any;
  pageNumber?: any;
  pageSize?: any;
  sortModel?:sortModel;
  filterConditionAndOr?:any;
}

export interface filterModel{
  filter:any;
  filterType:any;
  type:any;
}
export interface sortModel{
  colId:any;
  sortOrder:any;
}

export interface pageChange{
  page:any;
  pageSize:any;
  starRaw:any;
  endRaw:any
}

