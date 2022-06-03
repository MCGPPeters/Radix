param location string

@description('The name of the cognitive search service')
param searchServiceName string

resource cognitiveSearch 'Microsoft.Search/searchServices@2021-04-01-preview' ={
  location: location
  name: searchServiceName
  sku: {
    name: 'free'
  }
}

output searchApiKey string = listAdminKeys(searchServiceName, cognitiveSearch.apiVersion).primaryKey

