(function() {
'use strict';

    angular
        .module('fileUpload')
        .controller('ModalController', ModalController)
        .service('ModalService', ModalService);

    ModalService.$inject = ['$uibModal'];
    
    function ModalService($uibModal) {
        
        var _instance = {};
        var _params = {};
        
        this.show = show;
        
        function show(params, callbackOk, callbackCancel) {
            
            _params = params;
            _instance = $uibModal.open({
                animation: false,
                /*templateUrl: '/Areas/Video/App/Upload/video-upload-modal.html',*/
                template: '<div class="modal-header"> <h3 class="modal-title text-uppercase">Ocorreu um erro ao enviar alguns arquivos</h3> </div> <div class="modal-body"> <div class="panel panel-danger"> <div class="panel-heading">Lista de arquivos com falha</div> <ul class="list-group"> <li class="list-group-item" ng-repeat="video in vm.params"> <span>{{video.name}} - <small>{{video.size | bytes}}</small></span> </li> </ul> </div> </div> <div class="modal-footer"> <button class="btn btn-primary" type="button" ng-click="vm.ok()">Reenviar arquivos com falha</button> <button class="btn btn-warning" type="button" ng-click="vm.cancel()">Não reenviar arquivos com falha</button> </div>',
                controller: 'ModalController',
                controllerAs: 'vm',
                backdrop: 'static',
                keyboard: false,
                resolve: { params: function () { return _params; } }
            });
            
            _instance.result.then(callbackOk, callbackCancel);
            
        }
        
    }
    
    ModalController.$inject = ['$uibModalInstance', 'params'];
    
    function ModalController($uibModalInstance, params) {
        
        var vm = this;
        vm.ok = callbackOk;
        vm.cancel = callbackCancel;
        vm.params = params;
        
        function callbackOk(){
            $uibModalInstance.close();
        }
        function callbackCancel(){
            $uibModalInstance.dismiss();
        }
        
    }
    
})();