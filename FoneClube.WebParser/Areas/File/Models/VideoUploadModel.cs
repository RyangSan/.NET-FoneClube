using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoUpload.Utilidades;

namespace VideoUpload.Areas.Video.Model
{
    public class VideoUploadModel
    {

        internal bool UploadVideo(List<VideoModel> videos)
        {
            
            var amazonManager = new AmazonManager(Constants.awsAccessKey, Constants.awsSecretAccessKey);
            
            foreach (var video in videos)
            {
                if (video.NovaLista)
                    LoggerManager.Logger = new LoggerManager();

                var questaoId = Convert.ToInt32(video.Nome.Replace(Constants.extensao, string.Empty));
                var guid = string.Empty;

                try
                {
                    guid = new ApiGateway.VideoUpload(ApiGateway.TipoRetorno.Json).GetGuidQuestao(questaoId).Replace("\"", "");
                    if (string.Equals(guid, ""))
                    {
                        LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "erro"));
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "erro"));
                    return false;
                }
                
                var nomeFormatado = string.Format("{0}-{1}{2}", questaoId.ToString(), guid, Constants.extensao);

                if (amazonManager.UploadFile(Constants.VideoUploadBucket, video.ByteArrayVideo, string.Concat(Constants.VideoUploadSubfolder, nomeFormatado)))
                {
                    var jsonPost = string.Concat("{\"Id\":", questaoId, "}");
                    var setVideo = false;

                    try
                    {
                        setVideo = string.Equals(new ApiGateway.VideoUpload(ApiGateway.TipoRetorno.Json).SetVideoQuestao(jsonPost).ToString(), "1");
                        if (setVideo)
                            LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "ok"));
                        else
                            LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "erro"));
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "erro"));
                        return false;
                    }

                    return setVideo;
                }
                else
                {
                    LoggerManager.Logger.IncluirLog(string.Format("{0} | {1}", questaoId, "erro"));
                    return false;
                }

            }

            LoggerManager.Logger.IncluirLog(string.Format("sem vídeo enviado | {0}", "erro"));
            return false;
        }
    }

    public class VideoModel
    {
        public byte[] ByteArrayVideo { get; set; }
        public string Nome { get; set; }
        public bool NovaLista { get; set; }
        public bool SendReport { get; set; }
    }
}