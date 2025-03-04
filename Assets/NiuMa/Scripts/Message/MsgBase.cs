using System.Collections;
using System.Collections.Generic;

namespace NiuMa
{
    public abstract class MsgBase
    {
        public abstract string GetMsgType();

        public virtual MsgWrapper PackMessage() { throw new System.NotImplementedException("PackMessage method not implemented."); }
    }
}
