using System.Collections.Generic;

namespace MissingReferencesUtility
{
    public struct ResultContainer
    {
        public string ContainerName;
        public List<string> MissingComponents;
        public List<string> MissingReferences;

        public ResultContainer(string name, List<string> missingComponents, List<string> missingReferences)
        {
            ContainerName = name;
            MissingComponents = missingComponents;
            MissingReferences = missingReferences;
        }
    }
}