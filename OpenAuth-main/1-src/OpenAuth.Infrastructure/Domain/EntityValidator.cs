using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 对象验证基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityValidator<TEntity> : AbstractValidator<TEntity>
    {
        public void IsValid(TEntity entity)
        {
            var result = Validate(entity);
            if (!result.IsValid)
            {
                //抛出第一个验证错误
                throw new ValidatorException(result.Errors, 500);
            }
        }

        public bool Valid(TEntity entity, out List<string> errors)
        {
            var result = Validate(entity);
            errors = new List<string>();
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors.Count == 0;
        }
    }
}
