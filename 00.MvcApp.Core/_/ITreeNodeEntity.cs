using System.Collections.Generic;

namespace MvcApp.Core
{
    public interface ITreeNodeEntity
    {
        string Text { get; set; }
        string Value { get; set; }
        List<ITreeNodeEntity> Children { get; set; }
    }

    public class TreeNodeEntity : ITreeNodeEntity
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<ITreeNodeEntity> Children { get; set; }
    }
}
