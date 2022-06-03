param location string
param name string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: name
  location: location
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

output clientId string = logAnalyticsWorkspace.properties.customerId
output clientSecret string = logAnalyticsWorkspace.listKeys().primarySharedKey
