# DirectoryCleanerService

Serviço em .NET que limpa pastas monitoradas, removendo arquivos mais antigos que um determinado número de dias.

## 📋 Descrição

O **DirectoryCleanerService** é um _BackgroundService_ em C# que, periodicamente, percorre diretórios configurados e exclui arquivos cuja última modificação ultrapassa o tempo máximo definido. Ideal para automação de limpeza de logs, arquivos temporários ou qualquer coleção de arquivos que precise ser mantida sob controle.

## 🔧 Pré-requisitos

- [.NET SDK 7.0+](https://dotnet.microsoft.com/download)
- Acesso de leitura/gravação nas pastas configuradas
- arquivo `config.json` na raiz do executável

## ⚙️ Configuração

Crie um arquivo `config.json` ao lado do executável com este formato:

```json
{
  "CheckIntervalMinutes": 120,
  "DeleteFilesOlderThanDays": 30,
  "FoldersToClean": [
    "C:\\Temp\\Logs",
    "D:\\Dados\\ArquivosAntigos"
  ]
}
````

* `CheckIntervalMinutes` — intervalo em minutos entre cada limpeza.
* `DeleteFilesOlderThanDays` — idade mínima em dias para arquivos serem deletados.
* `FoldersToClean` — lista de caminhos absolutos para as pastas que serão limpas.

## 🚀 Como usar

1. **Clonar o repositório**

   ```bash
   git clone https://github.com/jaovitm/DirectoryCleanerService.git
   cd DirectoryCleanerService
   ```

2. **Restaurar dependências e compilar**

   ```bash
   dotnet restore
   dotnet build --configuration Release
   ```

3. **Publicar**

   ```bash
   dotnet publish -c Release -o ./publish
   ```

4. **Configurar**

   * Copie seu `config.json` para a pasta `publish`.

5. **Executar**

   ```bash
   cd publish
   dotnet DirectoryCleanerService.dll
   ```

## 📝 Como funciona

1. Ao iniciar, carrega as configurações de `config.json`.
2. Registra no log que o serviço foi iniciado e o intervalo de limpeza.
3. Cria um `Timer` que chama `CleanFolders()` imediatamente (`dueTime = 0`) e, a cada `CheckIntervalMinutes`.
4. Em cada execução de `CleanFolders()`:

   * Percorre cada pasta de `FoldersToClean`.
   * Verifica se a pasta existe; caso contrário, emite *warning*.
   * Lista arquivos, checa a diferença entre `DateTime.Now` e `LastWriteTime`.
   * Se a idade for maior que `DeleteFilesOlderThanDays`, exclui o arquivo e registra no log.

## 📊 Logging

O serviço utiliza `ILogger<Worker>` para:

* Informar início e conclusão de operações.
* Avisos de pastas não encontradas.
* Erros de carregamento de configuração ou exclusão de arquivos.

## 🛠️ Extensões & Customizações

* Adicionar suporte a filtros de nome de arquivo ou extensões.
* Enviar notificações via e-mail ou webhook após cada limpeza.
* Converter em um serviço Windows / systemd para inicialização automática.

---

> **Atenção:** teste sempre em um ambiente de desenvolvimento antes de rodar em produção, para evitar exclusões indesejadas!

