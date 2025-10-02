 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 验证参数
    /// </summary>
    public interface IValidateForm
    {
        void Validate();
    }

    public abstract class EntityInt : IEntity<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int Id { get; set; }
    }

    public abstract class EntityLong : IEntity<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long Id { get; set; }

    }

    public abstract class EntityString : IEntity<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual string Id { get; set; }
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

    public interface IEntity
    {

    }

    public interface ISoftDeleted
    {
        int IsDeleted { get; set; }
    }

    public interface ITenantId
    {
        long TenantId { get; set; }
    }
}
