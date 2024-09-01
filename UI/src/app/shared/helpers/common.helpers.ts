import { HttpClient } from "@angular/common/http";
import { publishFacade } from "@angular/compiler";
import { Injectable } from "@angular/core";
import { RequestWithFilterAndSort,filterModel } from "../models/FilterRequset";
import { filterConditionAndOrEnum } from "../constant/enum.const";

@Injectable({
  providedIn: 'root'
})

export class totalPageArray {
  GetTotalPageArray(totalItem: any, pageSize: any) {
    var totalPage = Math.ceil(totalItem / pageSize)
    var totalPages = []
    for (let i = 1; i <= totalPage; i++) {
      totalPages.push(i);
    }
    return totalPages;
  }
}


export class commonHelper {
  commonSearchWithinGrid(columns: any, searchText: string, existingRequest: RequestWithFilterAndSort) {
    // Sample grid columns array
    let gridColumns: string[] = columns;

    // Loop through gridColumns array
    for (let column of gridColumns) {
      // Create a new filter model for each column
      let newFilterModel: filterModel = {
        filterType: "text",
        type: "contains",
        filter: searchText,
      };
      existingRequest.filterConditionAndOr = filterConditionAndOrEnum.OrCondition;
      // Add the new filter model to the existingRequest filterModel object
      existingRequest.filterModel[column] = newFilterModel;
    }

    return existingRequest;
  }
}
