using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGameX
{
    public interface IIdentification
    {
        string Name { get; set; }
        string Description { get; set; }
        string ID { get; set; }
        string ToString();
    }
}
