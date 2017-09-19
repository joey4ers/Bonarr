﻿using System.Linq;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Indexers;

namespace NzbDrone.Core.HealthCheck.Checks
{
    public class IndexerRssCheck : HealthCheckBase
    {
        private readonly IIndexerFactory _indexerFactory;

        public IndexerRssCheck(IIndexerFactory indexerFactory)
        {
            _indexerFactory = indexerFactory;
        }

        public override HealthCheck Check()
        {
            var enabled = _indexerFactory.RssEnabled(false);

            if (enabled.Empty())
            {
                return new HealthCheck(GetType(), HealthCheckResult.Error, "No indexers available with RSS sync enabled, Bonarr will not grab new releases automatically");
            }

            var active = _indexerFactory.RssEnabled(true);

            if (active.Empty())
            {
                 return new HealthCheck(GetType(), HealthCheckResult.Warning, "All rss-capable indexers are temporarily unavailable due to recent indexer errors");
            }

            return new HealthCheck(GetType());
        }
    }
}
