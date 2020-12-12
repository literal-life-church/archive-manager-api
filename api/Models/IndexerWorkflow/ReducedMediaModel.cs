using System;
using System.Collections.Generic;

namespace LiteralLifeChurch.ArchiveManagerApi.Models.IndexerWorkflow
{
    public class ReducedMediaModel : IIndexerWorkflowModel
    {
        public DateTime Date;

        public DateModifierType DateModifier;

        public string MediaType;

        public string Name;

        public List<string> Speakers;
    }
}
