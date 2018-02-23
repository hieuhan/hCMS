CKEDITOR.plugins.add('media', {
    lang: 'vi,en',
    init: function (editor) {
        var iconPath = this.path + 'icons/media.png';

        editor.addCommand('mediaDialog', new CKEDITOR.dialogCommand('mediaDialog'));

        editor.ui.addButton('media', {
            label: 'Insert media',
            command: 'mediaDialog',
            icon: iconPath
        });
        CKEDITOR.dialog.add('mediaDialog', this.path + 'dialogs/media.js');
    }
});