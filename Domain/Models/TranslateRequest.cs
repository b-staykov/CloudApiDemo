using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TranslateRequest
    {        
        [Required(ErrorMessage = "Value is requered.")]
        [Range(1, 100000)]
        public int NumberOfTasks { get; set; }
        
        [MaxArraySize(100000)]
        public string[] TranslateItems { get; set; }
    }

    internal class ArraySize : ValidationAttribute
    {
        private readonly int _size;

        public ArraySize(int size)
        {
            _size = size;
        }
    }

    internal class MaxArraySize : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxArraySize(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (!value.GetType().IsArray)
            {
                return new ValidationResult($"Value is not an array.");
            }

            var arr = (object[])value;

            if (arr.Length == 0 || arr.Length > _maxSize)
            {
                return new ValidationResult($"Value must contain between 1 and {_maxSize} elements.");
            }

            return ValidationResult.Success;
        }
    }
}
