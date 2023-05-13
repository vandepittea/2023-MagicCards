using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace Howest.MagicCards.Shared.DTO
    {
        public class CardWriteDto
        {
            [Required(ErrorMessage = "The Name field is required.")]
            [StringLength(50, ErrorMessage = "The Name field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string Name { get; set; }

            [Required(ErrorMessage = "The ManaCost field is required.")]
            [StringLength(10, ErrorMessage = "The ManaCost field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string ManaCost { get; set; }

            [Required(ErrorMessage = "The Type field is required.")]
            [StringLength(50, ErrorMessage = "The Type field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string Type { get; set; }

            [Required(ErrorMessage = "The SetCode field is required.")]
            [StringLength(10, ErrorMessage = "The SetCode field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string SetCode { get; set; }

            [Required(ErrorMessage = "The RarityCode field is required.")]
            [StringLength(10, ErrorMessage = "The RarityCode field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string RarityCode { get; set; }

            [Required(ErrorMessage = "The Image field is required.")]
            [StringLength(255, ErrorMessage = "The Image field must be a string with a maximum length of {1}.", MinimumLength = 1)]
            public string Image { get; set; }

            [Required(ErrorMessage = "The CardColors field is required.")]
            [MinLength(1, ErrorMessage = "The CardColors field must contain at least one element.")]
            public IEnumerable<long> CardColors { get; set; }

            [Required(ErrorMessage = "The CardTypes field is required.")]
            [MinLength(1, ErrorMessage = "The CardTypes field must contain at least one element.")]
            public IEnumerable<long> CardTypes { get; set; }

            [StringLength(500, ErrorMessage = "The Text field must be a string with a maximum length of {1}.")]
            public string Text { get; set; }

            [StringLength(500, ErrorMessage = "The Flavor field must be a string with a maximum length of {1}.")]
            public string Flavor { get; set; }

            [StringLength(50, ErrorMessage = "The Number field must be a string with a maximum length of {1}.")]
            public string Number { get; set; }

            [StringLength(50, ErrorMessage = "The Power field must be a string with a maximum length of {1}.")]
            public string Power { get; set; }

            [StringLength(50, ErrorMessage = "The Toughness field must be a string with a maximum length of {1}.")]
            public string Toughness { get; set; }

            [StringLength(50, ErrorMessage = "The Layout field must be a string with a maximum length of {1}.")]
            public string Layout { get; set; }

            public int? MultiverseId { get; set; }

            [StringLength(255, ErrorMessage = "The OriginalImageUrl field must be a string with a maximum length of {1}.")]

            public string OriginalImageUrl { get; set; }

            [MaxLength(500, ErrorMessage = "Original text cannot exceed 500 characters")]
            public string OriginalText { get; set; }

            [MaxLength(100, ErrorMessage = "Original type cannot exceed 100 characters")]
            public string OriginalType { get; set; }

            [MaxLength(50, ErrorMessage = "MTG ID cannot exceed 50 characters")]
            public string MtgId { get; set; }

            [MaxLength(500, ErrorMessage = "Variations cannot exceed 500 characters")]
            public string Variations { get; set; }

            [MaxLength(100, ErrorMessage = "Artist name cannot exceed 100 characters")]
            public string ArtistName { get; set; }
        }
    }
}