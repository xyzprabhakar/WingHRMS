using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace projContext.DB.Masters
{
    public class tblFileMaster : d_ModifiedBy
    {
        [Key]
        [MaxLength(64)]
        public string FileId { get; set; }
        public byte[] File { get; set; }
        public enmFileType FileType { get; set; }
    }
}
