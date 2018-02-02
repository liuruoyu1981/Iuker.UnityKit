/**
 * 获得一个Jint图片实例
 */
interface IGetJintImage {
    (path: string): IJintImage;
}

/**
 * 获得一个Jint按钮实例
 */
interface IGetJintButton {
    (path: string): IJintButton;
}

interface IGetJintText {
    (path: string): IJintText;
}

interface IGetJintInputField {

    (path: string): IJintInputField;
}

interface IGetJintRawImage {

    (path: string): IJintRawImage;
}

interface IGetJintToggle {

    (path: string): IGetJintToggle;
}

interface IJintView {

    GetJintImage: IGetJintImage;
    GetJintButton: IGetJintButton;
    GetJintContainer: IGetContainer;
    GetJintText: IGetJintText;
    GetJintInputField: IGetJintInputField;
    GetJintRawImage: IGetJintRawImage;
    GetJintToggle: IGetJintToggle;

}