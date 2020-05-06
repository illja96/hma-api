using HMA.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HMA.DTO.ViewModels.Transactions
{
    public class TransactionCreationRequestViewModel : IValidatableObject
    {
        /// <summary>
        /// House id
        /// </summary>
        [Required]
        public string HouseId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public TransactionTypeViewModel Type { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Is monthly repeatable?
        /// </summary>
        [DefaultValue(false)]
        public bool IsMonthlyRepeatable { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        [RegularExpressionList("[a-z0-9-]+")]
        public List<string> Tags { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (EndDate.HasValue && !IsMonthlyRepeatable)
            {
                results.Add(new ValidationResult("End date should be provided only if this transaction is monthly repeatable", new[] { nameof(EndDate) }));
            }

            if (StartDate > EndDate)
            {
                results.Add(new ValidationResult("Start date is after End date", new[] { nameof(EndDate) }));
            }

            var tagsCount = Tags?.Count();
            var distinctTagsCount = Tags?.Distinct().Count();
            if (tagsCount != distinctTagsCount)
            {
                results.Add(new ValidationResult("Tags duplication", new[] { nameof(Tags) }));
            }

            return results;
        }
    }
}
