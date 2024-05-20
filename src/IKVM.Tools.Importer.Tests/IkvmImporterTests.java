package ikvm.tools.importer.tests;

public class IkvmImporterTests {

	@ikvm.lang.ModuleInitializer
	static void Init() {
		
	}

	public static String echo(String value) {
		return value;
	}

}
