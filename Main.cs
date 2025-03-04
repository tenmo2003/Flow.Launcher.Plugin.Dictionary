using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.Dictionary
{
    public class Dictionary : IPlugin
    {
        private PluginInitContext _context;
        private QueryService _queryService;

        public void Init(PluginInitContext context)
        {
            _context = context;
            _queryService = new QueryService();
        }

        public List<Result> Query(Query query)
        {
            return _queryService.Query(query.Search).GetAwaiter().GetResult();
        }
    }
}