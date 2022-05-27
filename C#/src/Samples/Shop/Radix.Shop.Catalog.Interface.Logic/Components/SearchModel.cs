﻿using System.Threading.Channels;
using Radix.Components;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public record SearchModel
{
    public SearchModel(Channel<SearchTerm> crawlingMessageChannel)
    {
        CrawlingMessageChannel = crawlingMessageChannel;
        Products = new List<ProductModel>();
    }
    public List<ProductModel> Products { get; internal set; }

    public string SearchTerm { get; internal set; }

    public Search<ProductModel> Search { get; set; }
    public Channel<SearchTerm> CrawlingMessageChannel { get; }
}
