CKEDITOR.plugins.add('jwplayer', {
    requires: ['dialog'],
    init: function (editor) {
        var pluginName = 'jwplayer';
        CKEDITOR.dialog.add(pluginName, this.path + 'dialogs/jwplayer.js');
        var allowed = 'object[classid,codebase,height,hspace,vspace,width];' +
				'param[name,value];' +
				'embed[height,hspace,pluginspage,allowfullscreen,src,type,vspace,width,flashvars,id,name]';
        
        editor.addCommand(pluginName, new CKEDITOR.dialogCommand(pluginName, {
            allowedContent: allowed      
        }));

        editor.ui.addButton('jwplayer', {
            label: 'jwplayer',
            command: pluginName,
            icon: this.path + 'jwplayer/jwPlayer.gif',
            toolbar: 'insert,20'
        });
    }
});