# Fix-SymbolicLink is called for every item in the directory structure.
# It makes a determination on whether the item is a SymbolicLink. If so, it recursively calls itself on the target.
# If the link is a link to a file, nothing happens. If the link is a link to a directory incorrectly linked as a link to a file,
# the link is converted to a directory symlink.

function Fix-SymbolicLink ($i, $p) {
	try {
		# parent references item, same item
		if ($i -eq $p) {
			return $i;
		}

		if ($i.LinkType -ne 'SymbolicLink') {
			return $i
		}

		if ($i.Attributes -ne 'Archive', 'ReparsePoint') {
			return $i
		}

		# recurse into target, attempting to fix it
		$t = gi $(Join-Path "$($i.DirectoryName)" "$($i.Target)")
		$t = Fix-SymbolicLink $t

		# is the target a directory?
		if ($t.Attributes -eq 'Directory') {
			$n = $i.FullName
			$t = $i.Target

			# remove original item and replace with mklink /D
			ri $i.FullName
			& cmd /c "mklink /D $n $t" | Write-Host

			# refresh item for recursive call
			return (gi $i.FullName)
		}
	} catch {
		Write-Host -Foreground Red -Background Black ($i.FullName + ": " + $_.Exception.Message)
	}

	return $i
}

gci -Recurse (gi $PSScriptRoot).Parent | %{ Fix-SymbolicLink $_ $null | Out-Null }
