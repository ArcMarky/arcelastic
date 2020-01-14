export class elasticListFilter {
  //isExactWords: boolean = false;
  //isSmartSearch: boolean = false;
  //isHighlightResults: boolean = false;
  //doesNotContain: boolean = false;
  filterTypeId: string;
  searchTerm: string = "";
}

export class elasticSearchResultsSummary {
  searchOutPut: elasticSearchResults[];
  hits: any;
  searchDuration: any;
}

export class elasticSearchResults {
  stringSearchResults: any;
  id: any;
  index: any;
  score: any;
}
