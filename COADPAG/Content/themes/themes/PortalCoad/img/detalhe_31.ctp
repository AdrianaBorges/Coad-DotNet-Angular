<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta name="google-site-verification" content="K5g_8iHVAJpC4xV4jTsnhReamlvyghiQWSJa0Trz7FM" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />		
		<meta name="robot" content="All" />
		<meta name="language" content="PT" />
		<meta name="robots" content="index,follow" />		
		<meta name="date" content="<?php echo $dados['Tab_31']['dataCadastro'];?>">
		<!--
		<PageMap>
			 <DataObject type="document">
				<Attribute name="id"><?php echo $dados['Tab_31']['id'];?></Attribute>
				<Attribute name="ranking"><?php echo $ranking; ?></Attribute>
				<Attribute name="last_update"><?php echo $dados['Tab_31']['dataCadastro'];?></Attribute>
			 </DataObject>
		</PageMap>
		-->		
        <link rel="stylesheet" type="text/css" href="/css/style.css"/>
		<link rel="stylesheet" type="text/css" href="/css/main2.css"/>
		<link rel="stylesheet" type="text/css" href="/css/facebox.css"/>
		
		<script type="text/javascript" src="/js/jquery.js"></script>
        <script type="text/javascript" src="/js/jquery.validate.js"></script>
        <script type="text/javascript" src="/js/jquery.mask.js"></script>
        <script type="text/javascript" src="/js/jquery.flash.js"></script>
        <script type="text/javascript" src="/js/facebox.js"></script>
        <script type="text/javascript" src="/js/divbox.js"></script>
        <script type="text/javascript" src="/js/jquery.cycle.min.js"></script>
        <script type="text/javascript" src="/js/jquery.config.js"></script>
        <script type="text/javascript" src="/js/funcoes.js"></script>
		
		
		<script type="text/javascript">
		  var _gaq = _gaq || [];
		  _gaq.push(['_setAccount', 'UA-185321-1']);
		  _gaq.push(['_setDomainName', '.coad.com.br']);
		  _gaq.push(['_trackPageview']);

		  (function() {
			var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
			ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
			var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
		  })();
		</script>		
		 
		<?php
		
			$html = $dados['Tab_31_html']['html'];
			$no_html = strip_tags($html, '<(.*?)>');
			$html = substr(trim($no_html) , 20 , 300).'...';
		
		if($label == 'Consultoria') {
			echo "<title>" . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " . $dados['Tab_31']['svb'] . "</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " . $dados['Tab_31']['svb'] . '" />';
			echo '<meta name="description" content="' . date('d/m/Y', strtotime($dados['Tab_31']['dataCadastro'])).' - COAD -'.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '1';
		} elseif($label == 'Atos_Legais') {
			echo "<title>" . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['gg'] . " - " . $dados['Tab_31']['vb'] . "</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['gg'] . " - " . $dados['Tab_31']['vb'] . '" />';
			echo '<meta name="description" content="COAD - '.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['expressao_ato'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '1';
		} elseif($label == 'Sumulas_e_Enunciados') {
			echo "<title>" . $dados['Tab_31']['colec'] . " - Súmula | Enunciado - " . $dados['Tab_31']['num'] . "</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['colec'] . " - Súmula | Enunciado - " . $dados['Tab_31']['num'] .'" />';
			echo '<meta name="description" content="' . date('d/m/Y', strtotime($dados['Tab_31']['dataCadastro'])).' - COAD -'.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['expressao_ato'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '1';
		} elseif($label == 'Orientacao') {
			echo "<title>" . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " . $dados['Tab_31']['svb'] . "</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " . $dados['Tab_31']['svb'] . '" />';
			echo '<meta name="description" content="' . date('d/m/Y', strtotime($dados['Tab_31']['dataCadastro'])).' - COAD -'.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '10';
		} elseif($label == 'Tabela_Pratica') {
			echo "<title>" . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " .$dados['Tab_31']['svb'] . "</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['colec'] . " - " . $dados['Tab_31']['vb'] . " - " .$dados['Tab_31']['svb'] . '" />';
			echo '<meta name="description" content="' . date('d/m/Y', strtotime($dados['Tab_31']['dataCadastro'])).' - COAD -'.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['expressao_ato'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '1';
		} else {
			echo "<title>" . $dados['Tab_31']['vb'] . " - " . $dados['Tab_31']['svb'] . " - COAD</title>";
			echo '<meta name="title" content="' . $dados['Tab_31']['vb'] . ' - ' . $dados['Tab_31']['svb'] . ' - COAD" />';
			echo '<meta name="description" content="' . date('d/m/Y', strtotime($dados['Tab_31']['dataCadastro'])).' - COAD -'.$html.'" />';
			echo '<meta name="keywords" content="' . $dados['Tab_31']['colec'] . ', ' . $dados['Tab_31']['gg'] . ',' . $dados['Tab_31']['vb'] . ', ' . $dados['Tab_31']['svb'] . ', '. $dados['Tab_31']['complemento'] . ', ' . $dados['Tab_31']['expressao_ato'] . ', ' . $dados['Tab_31']['org'] . '" />';
			$ranking = '1';
		}		
		?>
		<style>
		.popup { width: 560px; }
		.popup .close { display: none; } 
		</style>
    </head>
    <body>
		<script>
			$(document).ready(function(){
			  $("#sendAssi2").click(function(){
				$("#checaAssi2").submit();
			  });
			});
		</script>
		<!--
		<div id="modalAssinanteTravado" style="display:none;">
			<div class="modal">
				<div class="modal-header">
				  <h3 id="myModalLabel">Já sou Assinante</h3>
				</div>
				<form action="/login" id="checaAssi2" method="POST">
					<div class="modal-body">
						<div class="cadastro">
							<p class="lead">Entre e aproveite o conteúdo exclusivo. <br /></p>
							<div class="">
									
									<input type="text" name="data[Cliente][usuario]" placeholder="Login">
									<input type="password" name="data[Cliente][senha]" placeholder="Senha">
							</div>
						</div>
					</div>
					<div class="modal-footer">
						<a href="#" class="btn btn btn-inverse" style="color: #FFF; font-family: MS Shell Dlg; font-size: 14px; line-height: 22px;" onclick="jQuery(document).trigger('close.facebox');">Agora não</a>
						<a href="http://www.coad.com.br/cadastre-se/" target="_blank" class="btn btn-danger" style="color: #FFF; font-family: MS Shell Dlg; font-size: 14px; line-height: 22px;">Cadastro gratuito</a>
						<button class="btn btn btn-success" id="sendAssi2" onclick="document.getElementById('sendAssi2').submit()">Entrar</button>
					</div>
				</form>
			</div>
		</div>
		-->
		
		<div id="modalAssinanteTravado" style="display:none; width: width: 560px;">
			<div class="modal-header">
              <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
              <h3 id="myModalLabel">Acessar Conteúdo Exclusivo</h3>
            </div>
            <div class="modal-body">
                <div class="cadastro fifty" style="width: 32%;">
                    <p>Tenha <span>acesso gratuito</span> ao conteúdo exclusivo COAD</p>
                    <p class="big-lead"><span>por</span>10 dias!</p>
                    <p class="p-small">Experimente o mais prático e completo Portal de Inteligência Tributária e Contábil do país.</p>
                    <div class="">
                            <a href="http://www.coad.com.br/cadastre-se/" target="_blank"><button class="btn btn-danger">Cadastre-se agora</button></a>
                    </div>
                </div>
                <div class="cadastro fifty last" style="width: 32%;">
                    <p class="lead" style="font-size: 16px;">Já sou assinante</p>
                    <div class="">
                        <form action="/login" id="checaAssi" method="POST">
                            <input type="text" name="data[Cliente][usuario]" placeholder="Login" style="font-size: 12px;">
                            <input type="password" name="data[Cliente][senha]" placeholder="Senha" style="font-size: 12px;">
                            <button class="btn btn btn-success" id="sendAssi">Entrar</button>
                        </form>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn btn-inverse" data-dismiss="modal" aria-hidden="true" onClick="history.go(0)">Agora não</button>
            </div>
		</div>
        <div id="container">
            <div class="conteudo_popup">
                <?php echo $this->element('header_busca');?>
                <div class="cont">
                    <div class="left">
                        <div class="menu">
                            <a href="javascript:;" rel="<?php echo $id_tipo_conteudo;?>" id="<?php echo $id_conteudo;?>" class="meus_documentos_salvar" ><img src="/imagens/ico_salvar.jpg" alt="Salvar" width="49" height="46" border="0" /></a><br />
                            <a href="javascript:;" onclick="do_print();"><img src="/imagens/ico_imprimir_popup.jpg" alt="Imprimir" width="49" height="46" border="0" /></a><br />
                            <!--a href="#"><img src="/imagens/ico_enviar.jpg" alt="Enviar" width="49" height="46" border="0" /></a><br /-->
                        </div>
                    </div>

                    <div id="load"></div>
					
					<div class="right">
						<div class="informativo">
						Informativo <?php echo $dados['Tab_31']['informativo'];?> - Página <?php echo $dados['Tab_31']['pagina'];?> - Ano <?php echo $dados['Tab_31']['ano'];?><br />
						</div>
						<div class="texto">
							<?php 
								if((!empty($_COOKIE['coad_cli_nome']) or !empty($_COOKIE['cli_id'])) || ($_SERVER["REMOTE_ADDR"] == $_SERVER["SERVER_ADDR"])) {
								echo $dados['Tab_31_html']['html'];
								} 
								else
								{
								echo substr($dados['Tab_31_html']['html'], 0, 1000) . '</font> </p> </body> </html>...';
								//echo "<br /><br /><center><a href='#modalAssinanteTravado' rel='facebox'>Clique aqui para ter acesso à integra do conteúdo</center></a>";
								}
							?>
						</div>
					</div>

                    <div class="bug"></div>
                </div>
				<?php
				if(!($_SERVER["REMOTE_ADDR"] == $_SERVER["SERVER_ADDR"])) {
					echo $this->element('rodape_busca');
				}
				?>
    </body>	
</html>


