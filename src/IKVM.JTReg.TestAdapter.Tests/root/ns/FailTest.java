/* @test
 * @bug 8000001
 * @summary Fail
 */
public class FailTest {

    public static void main(String[] args) throws Exception {
        throw new Exception("fail");
    }

}
