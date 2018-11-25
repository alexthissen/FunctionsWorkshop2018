using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ServerlessFunctionsAppNETCore.Activities
{
    public static class QRCodeGeneratorActivity
    {
        [FunctionName(nameof(QRCodeGeneratorActivity))]
        public static async Task<string> Run([ActivityTrigger] HighScore score,
            ILogger log, Binder binder)
        {
            log.LogInformation($"Generating QR Code for high score {score.Score} by {score.Nickname}");

            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode($"{score.Nickname} scored {score.Score}", QRCodeGenerator.ECCLevel.H);
            QRCode code = new QRCode(data);

            var attributes = new Attribute[]
            {
                new BlobAttribute("azurefunctions-qrcode-images/" + score.Nickname,
                FileAccess.ReadWrite),
                new StorageAccountAttribute("azurefunctions-blobs")
            };

            CloudBlockBlob blob = await binder.BindAsync<CloudBlockBlob>(attributes);
            using (var stream = await blob.OpenWriteAsync())
            {
                Bitmap bitmap = code.GetGraphic(20, Color.Black, Color.White, true);
                bitmap.Save(stream, ImageFormat.Png);
            }

            return blob.StorageUri.PrimaryUri.AbsoluteUri;
        }
    }

}
