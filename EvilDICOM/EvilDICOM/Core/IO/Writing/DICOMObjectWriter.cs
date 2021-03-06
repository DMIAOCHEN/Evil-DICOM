using EvilDICOM.Core.Helpers;
using EvilDICOM.Core.Interfaces;

namespace EvilDICOM.Core.IO.Writing
{
    public class DICOMObjectWriter
    {
        public static bool IsFileMetaGroup(IDICOMElement el)
        {
            return el.Tag.Group == "0002";
        }

        public static void Write(DICOMBinaryWriter dw, DICOMWriteSettings settings, DICOMObject d, bool isSequenceItem = false)
        {
            for (int i = 0; i < d.Elements.Count; i++)
            {
                IDICOMElement el = d.Elements[i];
                if (!isSequenceItem) TransferSyntaxHelper.SetSyntax(d, settings.TransferSyntax);
                DICOMWriteSettings currentSettings = IsFileMetaGroup(el) ? settings.GetFileMetaSettings() : settings;
                if (GroupWriter.IsGroupHeader(el))
                {
                    int skip = GroupWriter.WriteGroup(dw, currentSettings, d, el);
                    i += skip;
                }
                else
                {
                    DICOMElementWriter.Write(dw, currentSettings, el);
                }
            }
        }
    }
}