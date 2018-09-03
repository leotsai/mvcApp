using System;
using System.Data;
using MvcApp.Core.Extensions;

namespace MvcApp.Core
{
    public interface ISimpleEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }

    public class SimpleEntity : ISimpleEntity, IQueryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public SimpleEntity()
        {
            
        }

        public SimpleEntity(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public virtual void Fill(DataRow row)
        {
            this.Id = row.Get<Guid>("Id");
            this.Name = row.Get<string>("Name");
        }
    }
}
