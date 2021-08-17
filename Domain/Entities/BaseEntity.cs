using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        private DateTime? _createAt;
        private DateTime? _updateAt;

        public DateTime? CreateAt {
            get { return _createAt; }
            set { _createAt = (value == null ? DateTime.UtcNow : value); }
        }

        public DateTime? UpdateAt
        {
            get { return _updateAt; }
            set { _updateAt = (value == null ? DateTime.UtcNow : value); }
        }


    }
}
