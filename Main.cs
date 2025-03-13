using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.FreeDictionary
{
    public class Dictionary : IAsyncPlugin
    {
        private PluginInitContext _context;
        private IPublicAPI _publicAPI;
        private QueryService _queryService;

        public async Task InitAsync(PluginInitContext context)
        {
            _publicAPI = context.API;
            _context = context;
            _queryService = new QueryService();
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            if (string.IsNullOrEmpty(query.Search))
            {
                return new List<Result> { new() { Title = "Enter the word you wish to conquer! ╰( ◕ ᗜ ◕ )╯", IcoPath = "icon.png" } };
            }

            await Task.Delay(400, token);

            if (token.IsCancellationRequested)
            {
                return null;
            }

            return await _queryService.Query(query.Search, _publicAPI);
        }
    }
}