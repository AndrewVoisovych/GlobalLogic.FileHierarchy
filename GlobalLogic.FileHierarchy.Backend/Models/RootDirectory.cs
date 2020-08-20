using System.Collections.Generic;

namespace GlobalLogic.FileHierarchy.Backend.Models
{
    public sealed class RootDirectory
    {
        public string Name { get; set; }
      
        public string DateCreated { get; set; }
        public List<Files> Files { get; set; }
        public List<RootDirectory> Children { get; set; }
    }
}