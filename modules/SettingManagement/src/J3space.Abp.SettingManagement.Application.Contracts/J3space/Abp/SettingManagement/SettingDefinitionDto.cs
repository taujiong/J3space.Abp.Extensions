namespace J3space.Abp.SettingManagement
{
    public class SettingDefinitionDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
    }
}