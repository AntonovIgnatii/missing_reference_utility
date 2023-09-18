using System;
using System.Collections.Generic;

namespace MissingReferencesUtility
{
    public interface IMissingReferenceFinder
    {
        SearchExecutor SearchExecutor();

        public List<ResultContainer> Find
        (
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete
        );
    }
}