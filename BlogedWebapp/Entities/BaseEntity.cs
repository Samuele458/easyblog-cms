﻿using BlogedWebapp.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogedWebapp.Entities
{

    public interface IBaseEntity
    {
        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }

    public interface IIdentificableEntity
    {

        public string Id { get; set; }
    }

    /// <summary>
    ///  Base entity
    /// </summary>
    public abstract class BaseEntity : IBaseEntity, IIdentificableEntity
    {
        [Projection(ProjectionBehaviour.Preview)]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Projection(ProjectionBehaviour.Preview)]
        public int Status { get; set; } = 1;

        [Projection(ProjectionBehaviour.Full)]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Projection(ProjectionBehaviour.Full)]
        public DateTime UpdatedOn { get; set; }
    }
}
