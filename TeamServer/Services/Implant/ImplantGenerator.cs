using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TeamServer.Services.Implant
{
    public static class ImplantGenerator
    {

        public static byte[] GenerateImplant(byte[] fileBytes, string bindHost, string bindPort, string listenerType, string[] classNames)
        {

            var modifyedImplant = ModifyConfigFields(fileBytes, bindHost, bindPort, listenerType);

            if (classNames == null || classNames.Length == 0)
            {
                return modifyedImplant;
            }
            var removeedCommandsImpalnt = RemoveClasses(modifyedImplant, classNames);
            return removeedCommandsImpalnt;
        }

        private static byte[] RemoveClasses(byte[] fileBytes, string[] classNames)
        {

            using (var memoryStream = new MemoryStream(fileBytes))
            {

                AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(memoryStream);
                ModuleDefinition module = assembly.MainModule;

                // Удаление классов из пространства имен "Implant.Commands" с указанными именами
                List<TypeDefinition> classesToRemove = module.Types
                    .Where(type => type.Namespace == "Implant.Commands" && classNames.Contains(type.Name))
                    .ToList();

                // Удаление классов из модуля
                foreach (TypeDefinition classToRemove in classesToRemove)
                {
                    module.Types.Remove(classToRemove);
                }

                using (var modifiedMemoryStream = new MemoryStream())
                {
                    assembly.Write(modifiedMemoryStream);
                    return modifiedMemoryStream.ToArray();
                }
            }
        }

        private static byte[] ModifyConfigFields(byte[] fileBytes, string newBindhost, string newBindport, string newListenerType)
        {
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                var assembly = AssemblyDefinition.ReadAssembly(memoryStream);

                var module = assembly.MainModule;

                // Получаем тип Config
                var configType = module.Types.FirstOrDefault(t => t.Namespace == "Implant" && t.Name == "Config");

                // Получаем поля
                var bindHostField = configType.Fields.FirstOrDefault(f => f.Name == "BindHost");
                var bindPortField = configType.Fields.FirstOrDefault(f => f.Name == "BindPort");
                var typeField = configType.Fields.FirstOrDefault(f => f.Name == "Type");

                // Получаем конструктор класса Config
                var configCtor = configType.Methods.FirstOrDefault(m => m.Name == ".cctor");

                // Получаем IL процессор для конструктора
                var ilProcessor = configCtor.Body.GetILProcessor();

                // Находим инструкции записи в статические поля
                var bindHostInstruction = ilProcessor.Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Stsfld && ((FieldDefinition)i.Operand).Name == "BindHost");
                var bindPortInstruction = ilProcessor.Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Stsfld && ((FieldDefinition)i.Operand).Name == "BindPort");
                var typeInstruction = ilProcessor.Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Stsfld && ((FieldDefinition)i.Operand).Name == "Type");

                // Получаем индексы инструкций
                var bindHostIndex = ilProcessor.Body.Instructions.IndexOf(bindHostInstruction);
                var bindPortIndex = ilProcessor.Body.Instructions.IndexOf(bindPortInstruction);
                var typeIndex = ilProcessor.Body.Instructions.IndexOf(typeInstruction);

                // Заменяем инструкции Ldstr для изменения значений полей
                ilProcessor.Body.Instructions[bindHostIndex - 1] = Instruction.Create(OpCodes.Ldstr, newBindhost);
                ilProcessor.Body.Instructions[bindPortIndex - 1] = Instruction.Create(OpCodes.Ldstr, newBindport);
                ilProcessor.Body.Instructions[typeIndex - 1] = Instruction.Create(OpCodes.Ldstr, newListenerType);

                using (var modifiedMemoryStream = new MemoryStream())
                {
                    // Сохраняем измененную сборку в поток
                    assembly.Write(modifiedMemoryStream);
                    return modifiedMemoryStream.ToArray();
                }
            }
        }
    }

}
