using System.Collections.Generic;

namespace GlobalLogic.FileHierarchy.Backend.Models
{
    public sealed class RootDirectory
    {
        public string Name { get; set; }
        //Todo: Convert to "10-Jun-18 5:59 PM",
        public string DateCreated { get; set; }
        public List<Files> Files { get; set; }
        public List<RootDirectory> Children { get; set; }
    }
}