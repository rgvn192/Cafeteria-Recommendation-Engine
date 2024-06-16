using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Common.Utils;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Services
{
    public abstract class CrudBaseService<T> : ICrudBaseService<T> where T : BaseEntity
    {
        #region Private Properties

        protected readonly IMapper _mapper;
        protected readonly ICrudBaseRepository<T> _crudBaseRepository;
        protected readonly ILogger _logger;

        //List of Properties that can be updated
        protected abstract List<string> ModifiableProperties
        {
            get;
        }

        //allows for setting associated collections that have to be loaded to be modified
        protected virtual string ModifiableInclude => null;

        #endregion Private Properties

        #region Constructor

        public CrudBaseService(ICrudBaseRepository<T> crudBaseRepository, IMapper mapper, ILogger logger)
        {
            _crudBaseRepository = crudBaseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Constructor

        #region Methods

        public virtual async Task<int> Add<TModel>(TModel model)
        {
            if (model == null)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                    { nameof(model), "null" }
                };

                throw new AppException(ErrorResponse.ErrorEnum.NullObject,
                    LogExtensions.GetLogMessage(nameof(Add), paramDict, nameof(model).GetNullLog()), null, _logger);
            }

            return await _crudBaseRepository.Add(_mapper.Map<TModel, T>(model));
        }

        public virtual async Task<int> GetCount(string filter = null, System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            return await _crudBaseRepository.GetCount(filter, predicate);
        }

        public virtual async Task<List<TModel>> GetList<TModel>(string include = null, string filter = null, List<string> sort = null, int limit = 0, int offset = 0, System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            return _mapper.Map<List<T>, List<TModel>>(await _crudBaseRepository.GetList(include, filter, sort, limit, offset, predicate));
        }

        public virtual async Task<TModel> GetById<TModel>(int id, string include = null)
        {
            if (id <= 0)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                    { nameof(id), id.ToString() },
                };

                throw new AppException(ErrorResponse.ErrorEnum.Validation,
                    LogExtensions.GetLogMessage(nameof(Update), paramDict, nameof(id).GetInvalidIntLog()), null, _logger);
            }

            var entity = await _crudBaseRepository.GetById(id, include);
            return _mapper.Map<T, TModel>(entity);
        }

        public virtual async Task<int> Update<TModel>(int entityId, TModel model, List<string> updatedProperties = null)
        {
            if (entityId <= 0 || model == null)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                    { nameof(entityId), entityId.ToString() },
                    { nameof(model), model == null?"null":JsonSerializer.Serialize(model) },
                };

                throw new AppException(ErrorResponse.ErrorEnum.Validation,
                    LogExtensions.GetLogMessage(nameof(Update), paramDict, "Invalid Id or Model".GetInvalidIntLog()), null, _logger);
            }

            T baseEntity = await _crudBaseRepository.GetById(entityId, ModifiableInclude);

            return await Update<TModel>(baseEntity, model, updatedProperties);
        }

        public virtual async Task<int> Update<TModel>(T baseEntity, TModel model, List<string> updatedProperties = null)
        {
            if (baseEntity == null)
            {
                throw new AppException(ErrorResponse.ErrorEnum.NotFound,
                    LogExtensions.GetLogMessage(nameof(Update), null, nameof(baseEntity).GetNullOrEmptyLog()), null, _logger);
            }

            ApplyProprertyUpdates<TModel>(baseEntity, model, updatedProperties);
            return await _crudBaseRepository.Update(baseEntity);
        }

        public virtual async Task<int> Delete(int id)
        {
            if (id <= 0)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                    { nameof(id), id.ToString() },
                };

                throw new AppException(ErrorResponse.ErrorEnum.Validation,
                    LogExtensions.GetLogMessage(nameof(Update), paramDict, nameof(id).GetInvalidIntLog()), null, _logger);
            }

            return await _crudBaseRepository.Delete(id);
        }

        public virtual async Task<int> Delete(T entity)
        {
            if (entity == null)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                  { nameof(entity), JsonSerializer.Serialize(entity) },
                };
                throw new AppException(ErrorResponse.ErrorEnum.NullObject,
                   LogExtensions.GetLogMessage(nameof(GetById), paramDict, nameof(entity).GetNullOrEmptyLog()), null, _logger);
            }

            return await _crudBaseRepository.Delete(entity);
        }

        public virtual async Task<int> DeleteRange(List<T> entities)
        {
            if (entities == null || entities.Count <= 0)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                   { nameof(entities), JsonSerializer.Serialize(entities) },
                };

                throw new AppException(ErrorResponse.ErrorEnum.NullObject,
                LogExtensions.GetLogMessage(nameof(DeleteRange), paramDict, nameof(entities).GetNullOrEmptyLog()), null, _logger);
            }

            return await _crudBaseRepository.DeleteRange(entities);
        }

        public virtual async Task<int> AddRange(List<T> entities)
        {
            if (entities == null || entities.Count <= 0)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                   { nameof(entities), JsonSerializer.Serialize(entities) },
                };

                throw new AppException(ErrorResponse.ErrorEnum.NullObject,
                LogExtensions.GetLogMessage(nameof(AddRange), paramDict, nameof(entities).GetNullOrEmptyLog()), null, _logger);
            }

            return await _crudBaseRepository.AddRange(entities);
        }

        public virtual async Task<int> UpdateRange(List<T> entities)
        {
            if (entities == null || entities.Count <= 0)
            {
                Dictionary<string, string> paramDict = new Dictionary<string, string>()
                {
                   { nameof(entities), JsonSerializer.Serialize(entities) },
                };

                throw new AppException(ErrorResponse.ErrorEnum.NullObject,
                LogExtensions.GetLogMessage(nameof(UpdateRange), paramDict, nameof(entities).GetNullOrEmptyLog()), null, _logger);
            }

            return await _crudBaseRepository.UpdateRange(entities);
        }

        #endregion Methods

        #region Protected Methods

        protected virtual void ApplyProprertyUpdates<TModel>(T entity, TModel model, List<string> updatedProperties = null)
        {
            var newEntity = _mapper.Map<TModel, T>(model);
            var updatable = updatedProperties != null && updatedProperties.Count > 0 ? ModifiableProperties.Intersect(updatedProperties, StringComparer.OrdinalIgnoreCase) : ModifiableProperties;
            var props = newEntity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (updatable.Contains(prop.Name))
                {
                    var val = prop.GetValue(newEntity);
                    var entityProp = entity.GetType().GetProperty(prop.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (entityProp != null)
                    {
                        entityProp.SetValue(entity, val);
                    }
                }
            }
        }

        #endregion Protected Methods
    }
}
