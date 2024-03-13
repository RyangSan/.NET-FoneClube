(function () {
    'use strict';

    angular
      .module('fileUpload')
      .controller('FileUploadController', VideoUploadController);

    VideoUploadController.$inject = ['$timeout', 'Upload', 'ModalService'];

    function VideoUploadController($timeout, $upload, Modal) {
        
        // fields
        var vm = this;
        vm.upload = upload;
        vm.remove = remove;
        vm.selected = selected;
        
        vm.sucessFiles = [];
        
        clear();
        
        var itemRemoved = {};
        var modal = {};
        
        // methods
        function clear(){
            vm.count = 0;
            vm.files = [];
            vm.errFiles = [];
            //vm.sucessFiles = [];
            vm.disableUpload = false;
            vm.uploading = false;
        }
        function clearFiles(list){
            list.forEach(function(item){
                delete item.loading;
                delete item.progress;
                delete item.success;
                delete item.error;
            })
            return list;
        }
        
        function selected(files, errFiles) {
            vm.files = files;
            vm.errFiles = [];
            vm.count = 0;
            vm.disableUpload = false;
        }
        function upload() {
            vm.disableUpload = true;
            
            vm.listSuccessShowed = false;
            vm.sucessFiles = [];
            
            uploadFile( vm.files[vm.count], true );
        }
        function remove(item) {
            itemRemoved = vm.files.indexOf(item);
            vm.files.splice(itemRemoved, 1);
            itemRemoved = {};
        }
        function uploadFile(file, novalista) {

            var lastFile = (vm.count == (vm.files.length - 1));

            file.loading = true;
            file.progress = 0;
            
            vm.uploading = true;
            
            file.upload = $upload.upload({
                url: window.rootUrl + 'File/Upload/Post',
                file: file,
                data: {
                    dataUpload: JSON.stringify({
                        Nome: file.name,
                        NovaLista: novalista,
                        SendReport: lastFile
                    })
                }
            });

            file.upload.then(function (response) {

                var result = (response.data == 'True');
                
                if(result)
                {
                    file.loading = false;
                    file.success = result;
                    file.error = !result;
                    vm.uploading = false;
                    vm.sucessFiles.push(file);
                }
                else 
                {
                    file.loading = false;
                    file.success = false;
                    file.error = true;
                    
                    vm.errFiles.push(file);
                }
                
                carregaProximoVideo();
            });

            file.upload.progress(function (evt) {
                file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
            });

            file.upload.catch(function (response) {
                
                file.loading = false;
                file.success = false;
                file.error = true;
                
                vm.errFiles.push(file);
                
                carregaProximoVideo();
            });

        }
        
        function carregaProximoVideo() {

            if (vm.count == vm.files.length - 1) {

                if (vm.errFiles.length > 0) {
                    Modal.show(vm.errFiles, clickOk, clickCancel);
                    return;
                }

                vm.listSuccessShowed = vm.sucessFiles.length > 0;
                vm.files = [];

            }
            else if (vm.count < vm.files.length - 1)
            {
                vm.count++;
                uploadFile(vm.files[vm.count], false);
            }

        }
        
        function clickOk(){
            var list = clearFiles( vm.errFiles );
            
            clear();
            
            //vm.listSuccessShowed = true;
            vm.files = list;
            vm.disableUpload = true;
            
            uploadFile( vm.files[vm.count], false );
        }
        
        function clickCancel(){
            
            vm.uploading = false;
            vm.disableUpload = true;
            
        }
        
    }
})();